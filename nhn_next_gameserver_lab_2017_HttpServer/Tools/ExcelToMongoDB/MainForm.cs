﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

using MongoDB.Driver;
using MongoDB.Bson;


namespace ExcelToMongoDB
{
    public partial class MainForm : Form
    {        
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        bool DataInsertToDB(string fileName, int version)
        {
            string mongoDBIP = ""; //"mongodb://localhost:27017/?maxPoolSize=200"
            string mongoDBDatabaseName = "";
            string mongoDBCollectionName = "";

            var stream = new FileStream(fileName, FileMode.Open);
            var excelReader = Excel.ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader.AsDataSet();
            DataTable table = result.Tables[0];

            // 기존 컬렉션에 있는 문서를 모두 지우고 새로 넣는다.
            var CollectionName = Path.GetFileNameWithoutExtension(fileName);

            var Datas = GetDBCollection<BsonDocument>(mongoDBIP, mongoDBDatabaseName, mongoDBCollectionName);
            Datas.DeleteManyAsync(new BsonDocument());


            try
            {
                string strTemp;
                int excelInt;
                double excelDouble;

                // Excel 1열, 2열을 제외하고 for문 시작
                for (int rCnt = 2; rCnt < table.Rows.Count; rCnt++)
                {
                    BsonDocument data = new BsonDocument();

                    if (string.IsNullOrEmpty(table.Rows[rCnt].ItemArray[0].ToString()))
                    {
                        break;
                    }

                    for (int cCnt = 0; cCnt < table.Columns.Count; cCnt++)
                    {
                        strTemp = table.Rows[1].ItemArray[cCnt].ToString();
                        var strTemp2 = table.Rows[rCnt].ItemArray[cCnt].ToString();

                        if (strTemp == "")
                            continue;

                        if (Int32.TryParse(strTemp2, out excelInt))
                        {
                            // string에서 double 값이 있으면 excelDouble로 추출
                            data.Add(strTemp, excelInt);
                        }
                        else if (Double.TryParse(strTemp2, out excelDouble))
                        {
                            // string에서 double 값이 있으면 excelDouble로 추출
                            data.Add(strTemp, excelDouble);
                        }
                        else
                        {
                            // string에서 double 값이 없으면 strTemp2로 string 추출
                            data.Add(strTemp, strTemp2);
                        }
                    }

                    Datas.InsertOneAsync(data).Wait();
                }


                //var propertyData = new DBDataProperty();
                //propertyData._id = CollectionName; //dataName;

                //for (int cCnt = 0; cCnt < table.Columns.Count; cCnt++)
                //{
                //    var exportMark = table.Rows[0].ItemArray[cCnt].ToString();
                //    if (exportMark == "C" || exportMark == "CS")
                //    {
                //        var fieldName = table.Rows[1].ItemArray[cCnt].ToString();
                //        propertyData.ClientExportField.Add(fieldName);
                //    }
                //}

                //CreateDataProperty(propertyData);

            }
            catch (Exception ex)
            {
                //alertMessage = e.ToString();

                //var msg_E = "\r\n-------------------------------------------------------" +
                //"\r\n예외 발생 일자 : " + DateTime.Now.ToString() + e;

                //string path_E = Server.MapPath("~/Temp/") + "log.txt";

                //StreamWriter tw_E = System.IO.File.AppendText(path_E);
                //tw_E.WriteLine(msg_E);
                //tw_E.Close();

                return false;
            }

            excelReader.Close();

            return true;

        }

       

        static IMongoCollection<T> GetDBCollection<T>(string dbIP, string dbName, string collectionName)
        {
            var database = GetMongoDatabase(dbIP, dbName); 
            var collection = database.GetCollection<T>(collectionName);
            return collection;
        }

        static IMongoDatabase GetMongoDatabase(string dbIP, string dbName)
        {
            MongoClient cli = new MongoClient(dbIP);
            return cli.GetDatabase(dbName);
        }
                
    }
}
