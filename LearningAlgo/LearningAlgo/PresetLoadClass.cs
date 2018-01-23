using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningAlgo
{

    public class PresetLoadClass
    {
        /// <summary>
        /// プリセットテーブルの格納
        /// </summary>
        public Dictionary<string, Flowtable> PreTb;

        public async Task<Dictionary<string, Flowtable>> OnAppearing()
        {
            using (var connection = await CreateConnection())
            /* DBへのコネクションを取得してくるConnection()) */
            {
                //var preset =  connection.Table<FlowTable>();
                PreTb = new Dictionary<string, Flowtable>();

                foreach (var preset in connection.Table<FlowTable>())
                {
                    PreTb[preset.flow_id] = new Flowtable
                    {
                        flow_id = preset.flow_id,
                        flow_name = preset.flow_name,
                        comment = preset.comment
                    };
                }
                System.Diagnostics.Debug.WriteLine(PreTb);
                connection.Close();
            }
            return PreTb;
        }
        public async Task<SQLiteConnection> CreateConnection()
        {
            const string DatabasesFileName = "DB_TestFile.db3";

            /* ルートフォルダの取得を行う */
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            /* DBファイルの存在をチェックする */ 
            var result = await rootFolder.CheckExistsAsync(DatabasesFileName);
            if (result == ExistenceCheckResult.NotFound)
            {
                /* 存在しなかった場合に限り、新たにファイル作成とテーブル作成を新規に行う */
                IFile file = await rootFolder.CreateFileAsync(DatabasesFileName, CreationCollisionOption.ReplaceExisting);
                var connection = new SQLiteConnection(file.Path);

                connection.CreateTable<FlowTable>();
                connection.CreateTable<FlowPartsTable>();
                connection.CreateTable<OutputTable>();

                return connection;
            }
            else
            {
                /* ファイルが存在する場合はそのままコネクションの作成を行う */
                IFile file = await rootFolder.CreateFileAsync(DatabasesFileName, CreationCollisionOption.OpenIfExists);
                var connection = new SQLiteConnection(file.Path);

                /* テーブルが存在するか確認 */
                if (connection.GetTableInfo("DBtestDeta ").Count <= 0)
                {
                    connection.CreateTable<FlowTable>();
                    connection.CreateTable<FlowPartsTable>();
                    connection.CreateTable<OutputTable>();

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
/*
 foreach (var items in (from x in connection.Table<TypeTable>() orderby x.type_id select x))
                {
                    System.Diagnostics.Debug.WriteLine("flowTable" + items.type_id + items.type_id + items.output);
                }
                foreach (var items in (from x in connection.Table<FlowPartsTable>() orderby x.flow_id select x))
                {
                    System.Diagnostics.Debug.WriteLine("flowTable" + items.flow_id + items.identification_id + items.type_id + items.data + items.position + items.startFlag);
                }
                foreach (var items in (from x in connection.Table<OutputTable>() orderby x.flow_id select x))
                {
                    System.Diagnostics.Debug.WriteLine("flowTable" + items.flow_id + items.identification_id + items.output_identification_id);
                }
     
 */