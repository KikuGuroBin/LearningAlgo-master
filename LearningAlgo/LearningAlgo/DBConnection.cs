﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using PCLStorage;

namespace LearningAlgo
{
    public class DBConnection
    {

        public void DBRead()
        {
            OnAppearing();
        }


        public async void OnAppearing()
        {
            using (var connection = await CreateConnection())
            // DBへのコネクションを取得してくるConnection())
            {
                /*******テストデータ*********/
                /*flowデータ*/
                connection.Insert(new FlowTable { flow_id = "1", flow_name = "1", comment = "test" });
                connection.Insert(new FlowTable { flow_id = "2", flow_name = "2", comment = "quizeTest" });
                connection.Insert(new FlowTable { flow_id = "3", flow_name = "3", comment = "test2" });
                connection.Insert(new FlowTable { flow_id = "4", flow_name = "4", comment = "test3" });
                connection.Insert(new FlowTable { flow_id = "5", flow_name = "5", comment = "loopTest" });
                connection.Insert(new FlowTable { flow_id = "6", flow_name = "6", comment = "loopTest2" });

                /*flowPartsデータ*/
                connection.Insert(new FlowPartsTable { flow_id = "1", identification_id = "1", type_id = "SideSikaku.png", data = "1→i", position_X = "10", position_Y = "90", startFlag = "1" });
                connection.Insert(new FlowPartsTable { flow_id = "1", identification_id = "2", type_id = "SideSikaku.png", data = "2→j", position_X = "10", position_Y = "180", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "1", identification_id = "3", type_id = "SideSikaku.png", data = "j＋1→j", position_X = "10", position_Y = "270", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "1", identification_id = "4", type_id = "SideSikaku.png", data = "i＋1→i", position_X = "10", position_Y = "360", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "1", identification_id = "5", type_id = "SideHisigata.png", data = "i≦3", position_X = "10", position_Y = "450", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "1", identification_id = "6", type_id = "SideHeikou.png", data = "j出力", position_X = "10", position_Y = "540", startFlag = "0" });

                connection.Insert(new FlowPartsTable { flow_id = "2", identification_id = "1", type_id = "SideSikaku.png", data = "1→i", position_X = "10", position_Y = "90", startFlag = "1" });
                connection.Insert(new FlowPartsTable { flow_id = "2", identification_id = "2", type_id = "SideSikaku.png", data = "2→j", position_X = "10", position_Y = "180", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "2", identification_id = "3", type_id = "SideSikaku.png", data = "j＋1→j", position_X = "10", position_Y = "270", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "2", identification_id = "4", type_id = "SideSikaku.png", data = "i＋1→i", position_X = "10", position_Y = "360", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "2", identification_id = "5", type_id = "SideHisigata.png", data = "i≦3", position_X = "10", position_Y = "450", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "2", identification_id = "6", type_id = "SideHeikou.png", data = "j出力", position_X = "10", position_Y = "540", startFlag = "0" });

                connection.Insert(new FlowPartsTable { flow_id = "3", identification_id = "1", type_id = "SideSikaku.png", data = "1→i", position_X = "10", position_Y = "90", startFlag = "1" });
                connection.Insert(new FlowPartsTable { flow_id = "3", identification_id = "2", type_id = "SideSikaku.png", data = "2→j", position_X = "10", position_Y = "180", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "3", identification_id = "3", type_id = "SideSikaku.png", data = "j＋1→j", position_X = "10", position_Y = "270", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "3", identification_id = "4", type_id = "SideSikaku.png", data = "i＋1→i", position_X = "10", position_Y = "360", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "3", identification_id = "5", type_id = "SideHisigata.png", data = "i≦5", position_X = "10", position_Y = "450", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "3", identification_id = "6", type_id = "SideHeikou.png", data = "j出力", position_X = "10", position_Y = "540", startFlag = "0" });

                connection.Insert(new FlowPartsTable { flow_id = "4", identification_id = "1", type_id = "SideSikaku.png", data = "1→i", position_X = "130", position_Y = "90", startFlag = "1" });
                connection.Insert(new FlowPartsTable { flow_id = "4", identification_id = "2", type_id = "SideSikaku.png", data = "2→j", position_X = "130", position_Y = "180", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "4", identification_id = "3", type_id = "SideHisigata.png", data = "i＜3", position_X = "130", position_Y = "270", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "4", identification_id = "4", type_id = "SideHeikou.png", data = "j出力", position_X = "30", position_Y = "270", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "4", identification_id = "5", type_id = "SideSikaku.png", data = "i＋1→i", position_X = "130", position_Y = "360", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "4", identification_id = "6", type_id = "SideSikaku.png", data = "j＋1→j", position_X = "130", position_Y = "450", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "4", identification_id = "7", type_id = "SideHisigata.png", data = "j＜3", position_X = "130", position_Y = "540", startFlag = "0" });

                connection.Insert(new FlowPartsTable { flow_id = "5", identification_id = "1", type_id = "SideSikaku.png", data = "1→i", position_X = "10", position_Y = "90", startFlag = "1" });
                connection.Insert(new FlowPartsTable { flow_id = "5", identification_id = "2", type_id = "SideSikaku.png", data = "2→j", position_X = "10", position_Y = "180", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "5", identification_id = "3", type_id = "SideDaikeiUe.png", data = "i≧3", position_X = "10", position_Y = "270", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "5", identification_id = "4", type_id = "SideSikaku.png", data = "i＋1→i", position_X = "10", position_Y = "360", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "5", identification_id = "5", type_id = "SideSikaku.png", data = "j＋1→j", position_X = "10", position_Y = "450", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "5", identification_id = "6", type_id = "SideDaikeiSita.png", data = "", position_X = "10", position_Y = "540", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "5", identification_id = "7", type_id = "SideHeikou.png", data = "j出力", position_X = "10", position_Y = "630", startFlag = "0" });

                connection.Insert(new FlowPartsTable { flow_id = "6", identification_id = "1", type_id = "SideSikaku.png", data = "1→i", position_X = "10", position_Y = "90", startFlag = "1" });
                connection.Insert(new FlowPartsTable { flow_id = "6", identification_id = "2", type_id = "SideSikaku.png", data = "2→j", position_X = "10", position_Y = "180", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "6", identification_id = "3", type_id = "SideDaikeiUe.png", data = "", position_X = "10", position_Y = "270", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "6", identification_id = "4", type_id = "SideSikaku.png", data = "i＋1→i", position_X = "10", position_Y = "360", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "6", identification_id = "5", type_id = "SideSikaku.png", data = "j＋1→j", position_X = "10", position_Y = "450", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "6", identification_id = "6", type_id = "SideDaikeiSita.png", data = "i≧3", position_X = "10", position_Y = "450", startFlag = "0" });
                connection.Insert(new FlowPartsTable { flow_id = "6", identification_id = "7", type_id = "SideHeikou.png", data = "j出力", position_X = "10", position_Y = "630", startFlag = "0" });


                /*outputデータ*/
                connection.Insert(new OutputTable { flow_id = "1", identification_id = "1", output_identification_id = "2", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "1", identification_id = "2", output_identification_id = "3", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "1", identification_id = "3", output_identification_id = "4", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "1", identification_id = "4", output_identification_id = "5", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "1", identification_id = "5", output_identification_id = "3", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "1", identification_id = "5", output_identification_id = "6", blanch_flag = "-1" });
                connection.Insert(new OutputTable { flow_id = "1", identification_id = "6", output_identification_id = "-1", blanch_flag = "0" });

                connection.Insert(new OutputTable { flow_id = "2", identification_id = "1", output_identification_id = "2", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "2", identification_id = "2", output_identification_id = "3", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "2", identification_id = "3", output_identification_id = "4", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "2", identification_id = "4", output_identification_id = "5", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "2", identification_id = "5", output_identification_id = "3", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "2", identification_id = "5", output_identification_id = "6", blanch_flag = "-1" });
                connection.Insert(new OutputTable { flow_id = "2", identification_id = "6", output_identification_id = "-1", blanch_flag = "0" });

                connection.Insert(new OutputTable { flow_id = "3", identification_id = "1", output_identification_id = "2", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "3", identification_id = "2", output_identification_id = "3", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "3", identification_id = "3", output_identification_id = "4", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "3", identification_id = "4", output_identification_id = "5", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "3", identification_id = "5", output_identification_id = "3", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "3", identification_id = "5", output_identification_id = "6", blanch_flag = "-1" });
                connection.Insert(new OutputTable { flow_id = "3", identification_id = "6", output_identification_id = "-1", blanch_flag = "0" });

                connection.Insert(new OutputTable { flow_id = "4", identification_id = "1", output_identification_id = "2", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "4", identification_id = "2", output_identification_id = "3", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "4", identification_id = "3", output_identification_id = "5", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "4", identification_id = "3", output_identification_id = "4", blanch_flag = "-1" });
                connection.Insert(new OutputTable { flow_id = "4", identification_id = "5", output_identification_id = "6", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "4", identification_id = "6", output_identification_id = "7", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "4", identification_id = "7", output_identification_id = "3", blanch_flag = "-1" });
                connection.Insert(new OutputTable { flow_id = "4", identification_id = "7", output_identification_id = "6", blanch_flag = "0" });

                connection.Insert(new OutputTable { flow_id = "5", identification_id = "1", output_identification_id = "2", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "5", identification_id = "2", output_identification_id = "3", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "5", identification_id = "3", output_identification_id = "4", blanch_flag = "-1" });
                connection.Insert(new OutputTable { flow_id = "5", identification_id = "3", output_identification_id = "7", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "5", identification_id = "4", output_identification_id = "5", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "5", identification_id = "5", output_identification_id = "6", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "5", identification_id = "6", output_identification_id = "3", blanch_flag = "0" });

                connection.Insert(new OutputTable { flow_id = "6", identification_id = "1", output_identification_id = "2", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "6", identification_id = "2", output_identification_id = "3", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "6", identification_id = "3", output_identification_id = "4", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "6", identification_id = "4", output_identification_id = "5", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "6", identification_id = "5", output_identification_id = "6", blanch_flag = "0" });
                connection.Insert(new OutputTable { flow_id = "6", identification_id = "6", output_identification_id = "3", blanch_flag = "-1" });
                connection.Insert(new OutputTable { flow_id = "6", identification_id = "6", output_identification_id = "7", blanch_flag = "0" });

                /*typeデータ*/
                connection.Insert(new TypeTable { type_id = "SideSikaku.png", type_name = "1", output = "1" });
                connection.Insert(new TypeTable { type_id = "SideHisigata.png", type_name = "2", output = "2" });
                connection.Insert(new TypeTable { type_id = "SideDaikeiUe.png", type_name = "3", output = "1" });
                connection.Insert(new TypeTable { type_id = "SideDaikeiSita.png", type_name = "4", output = "1" });
                connection.Insert(new TypeTable { type_id = "SideHeikou.png", type_name = "5", output = "1" });

                /*quizeテーブルのデータ*/
                connection.Insert(new QuizTable { quiz_flow_id = "1", flow_id = "2" });

                /*spaceFlowPartsデータ*/
                connection.Insert(new SpaceIdentificationTable { quiz_flow_id = "1", space_identification_id = "2" });
                connection.Insert(new SpaceIdentificationTable { quiz_flow_id = "1", space_identification_id = "5" });



            }
        }


        public static async Task<SQLiteConnection> CreateConnection()
        {

            const string DatabasesFileName = "DB_TestFile.db3";
            /*ルートフォルダの取得を行う*/
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            /*DBファイルの存在をチェックする*/
            var result = await rootFolder.CheckExistsAsync(DatabasesFileName);
            if (result == ExistenceCheckResult.NotFound)
            {
                /*存在しなかった場合に限り、新たにファイル作成とテーブル作成を新規に行う*/
                IFile file = await rootFolder.CreateFileAsync(DatabasesFileName, CreationCollisionOption.ReplaceExisting);
                var connection = new SQLiteConnection(file.Path);

                connection.CreateTable<FlowTable>();
                connection.CreateTable<FlowPartsTable>();
                connection.CreateTable<OutputTable>();
                connection.CreateTable<TypeTable>();
                connection.CreateTable<QuizTable>();
                connection.CreateTable<SpaceIdentificationTable>();

                return connection;
            }
            else
            {
                /*ファイルが存在する場合はそのままコネクションの作成を行う*/
                IFile file = await rootFolder.CreateFileAsync(DatabasesFileName, CreationCollisionOption.OpenIfExists);
                ///<summary>
                ///データベース接続
                /// </summary>
                var connection = new SQLiteConnection(file.Path);

                /*テーブルを1回消去する*/
                connection.DropTable<FlowTable>();
                connection.DropTable<FlowPartsTable>();
                connection.DropTable<OutputTable>();
                connection.DropTable<TypeTable>();
                connection.DropTable<QuizTable>();
                connection.DropTable<SpaceIdentificationTable>();


                /*テーブルを新たに作成する*/
                if (connection.GetTableInfo("DBtestDeta ").Count <= 0)
                {
                    connection.CreateTable<FlowTable>();
                    connection.CreateTable<FlowPartsTable>();
                    connection.CreateTable<OutputTable>();
                    connection.CreateTable<TypeTable>();
                    connection.CreateTable<QuizTable>();
                    connection.CreateTable<SpaceIdentificationTable>();

                    return connection;
                }
                else
                {
                    return connection;
                }
            }
        }
    }
}