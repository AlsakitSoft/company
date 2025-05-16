using company.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace company.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string _connStr;

        public CompanyRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("Connection");
        }

        public async Task<IEnumerable<CompanyItem>> GetAllCompaniesAsync()
        {
            var list = new List<CompanyItem>();
            using var conn = new MySqlConnection(_connStr);
            await conn.OpenAsync();

            const string sql = @"
        SELECT 
            COMID,
            COMARABICNAME,
            COMENGLISHNAME,
            SHORTARABICNAME,
            SHORTENGLISHNAME,
            COMWEBSITE,
            COMADDRESS,
            COMNOTE,
            ComDefault,
            ADDUSER,
            ADDDATE,
            EDITUSER,
            EDITDATE,
            ISDELETED
        FROM COMPANYItem
        WHERE ISDELETED = 0;
    ";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            // دالة مساعدة لقراءة النصوص أو إرجاع null
            string SafeGetString(string colName) =>
                reader.IsDBNull(reader.GetOrdinal(colName))
                    ? null
                    : reader.GetString(reader.GetOrdinal(colName));

            // دالة مساعدة لقراءة nullable int
            int? SafeGetInt(string colName) =>
                reader.IsDBNull(reader.GetOrdinal(colName))
                    ? (int?)null
                    : reader.GetInt32(reader.GetOrdinal(colName));

            // دالة مساعدة لقراءة nullable DateTime
            DateTime? SafeGetDateTime(string colName) =>
                reader.IsDBNull(reader.GetOrdinal(colName))
                    ? (DateTime?)null
                    : reader.GetDateTime(reader.GetOrdinal(colName));

            while (await reader.ReadAsync())
            {
                list.Add(new CompanyItem
                {
                    ComId = reader.GetInt32(reader.GetOrdinal("COMID")),
                    ComArabicName = SafeGetString("COMARABICNAME"),
                    ComEnglishName = SafeGetString("COMENGLISHNAME"),
                    ShortArabicName = SafeGetString("SHORTARABICNAME"),
                    ShortEnglishName = SafeGetString("SHORTENGLISHNAME"),
                    ComWebsite = SafeGetString("COMWEBSITE"),
                    ComAddress = SafeGetString("COMADDRESS"),
                    ComNote = SafeGetString("COMNOTE"),
                    IsDefault = reader.GetBoolean(reader.GetOrdinal("ComDefault")),
                    AddUserId = reader.GetInt32(reader.GetOrdinal("ADDUSER")),
                    AddDate = reader.GetDateTime(reader.GetOrdinal("ADDDATE")),
                    EditUserId = SafeGetInt("EDITUSER"),
                    EditDate = SafeGetDateTime("EDITDATE"),
                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("ISDELETED"))
                });
            }

            return list;
        }


        public async Task SaveCompanyAsync(CompanyItem company)
        {
            using var conn = new MySqlConnection(_connStr);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("AddCompany", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // إضافة المعلمات للإجراء المخزن
            cmd.Parameters.AddWithValue("p_COMARABICNAME", (object)company.ComArabicName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_COMENGLISHNAME", (object)company.ComEnglishName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_SHORTARABICNAME", (object)company.ShortArabicName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_SHORTENGLISHNAME", (object)company.ShortEnglishName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_COMWEBSITE", (object)company.ComWebsite ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_COMADDRESS", (object)company.ComAddress ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_COMNOTE", (object)company.ComNote ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_ComDefault", company.IsDefault ? 1 : 0);
            cmd.Parameters.AddWithValue("p_ADDUSER", company.AddUserId); // استبدال القيمة الثابتة بمعرف المستخدم الحقيقي

            await cmd.ExecuteNonQueryAsync();
        }
    }
}


//CREATE TABLE COMPANYItem(
//    COMID INT AUTO_INCREMENT PRIMARY KEY,
//    COMARABICNAME VARCHAR(100),
//    COMENGLISHNAME VARCHAR(100),
//    SHORTARABICNAME VARCHAR(50),
//    SHORTENGLISHNAME VARCHAR(50),
//    COMWEBSITE VARCHAR(50),
//    COMADDRESS VARCHAR(150),
//    COMNOTE VARCHAR(150),
//    ComDefault TINYINT DEFAULT 0,
//    ADDUSER INT,
//    ADDDATE DATETIME DEFAULT CURRENT_TIMESTAMP,
//    EDITUSER INT,
//    EDITDATE DATETIME DEFAULT CURRENT_TIMESTAMP,
//    ISDELETED TINYINT DEFAULT 0
//);


//DELIMITER $$

//CREATE PROCEDURE AddCompany(
//    IN p_COMARABICNAME VARCHAR(100),
//    IN p_COMENGLISHNAME VARCHAR(100),
//    IN p_SHORTARABICNAME VARCHAR(50),
//    IN p_SHORTENGLISHNAME VARCHAR(50),
//    IN p_COMWEBSITE VARCHAR(50),
//    IN p_COMADDRESS VARCHAR(150),
//    IN p_COMNOTE VARCHAR(150),
//    IN p_ComDefault TINYINT,
//    IN p_ADDUSER INT
//)
//BEGIN
//    INSERT INTO COMPANYItem(
//        COMARABICNAME, COMENGLISHNAME, SHORTARABICNAME, SHORTENGLISHNAME,
//        COMWEBSITE, COMADDRESS, COMNOTE, ComDefault, ADDUSER
//    ) VALUES(
//        p_COMARABICNAME, p_COMENGLISHNAME, p_SHORTARABICNAME, p_SHORTENGLISHNAME,
//        p_COMWEBSITE, p_COMADDRESS, p_COMNOTE, p_ComDefault, p_ADDUSER
//    );
//END $$

//DELIMITER ;

//https://chat.qwen.ai/s/db524ab4-5280-4dfb-8f31-779a0f201873?fev=0.0.93