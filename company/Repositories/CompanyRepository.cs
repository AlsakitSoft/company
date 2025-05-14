using company.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
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

        //public async Task SaveCompanyAsync(CompanyItem company)
        //{
        //    using var conn = new MySqlConnection(_connStr);
        //    await conn.OpenAsync();

        //    var cmd = new MySqlCommand(
        //        @"INSERT INTO COMPANYItem (
        //            COMARABICNAME, COMENGLISHNAME, SHORTARABICNAME, SHORTENGLISHNAME, 
        //            COMWEBSITE, COMADDRESS, COMNOTE, ComDefault, ADDUSER
        //        ) VALUES (
        //            @arabicName, @englishName, @shortArabic, @shortEnglish, 
        //            @website, @address, @note, @isDefault, @addUser
        //        )", conn);

        //    cmd.Parameters.AddWithValue("@arabicName", (object)company.ComArabicName ?? DBNull.Value);
        //    cmd.Parameters.AddWithValue("@englishName", (object)company.ComEnglishName ?? DBNull.Value);
        //    cmd.Parameters.AddWithValue("@shortArabic", (object)company.ShortArabicName ?? DBNull.Value);
        //    cmd.Parameters.AddWithValue("@shortEnglish", (object)company.ShortEnglishName ?? DBNull.Value);
        //    cmd.Parameters.AddWithValue("@website", (object)company.ComWebsite ?? DBNull.Value);
        //    cmd.Parameters.AddWithValue("@address", (object)company.ComAddress ?? DBNull.Value);
        //    cmd.Parameters.AddWithValue("@note", (object)company.ComNote ?? DBNull.Value);
        //    cmd.Parameters.AddWithValue("@isDefault", company.IsDefault ? 1 : 0);
        //    cmd.Parameters.AddWithValue("@addUser", 1); // يجب استبداله بمعرف المستخدم الحقيقي

        //    await cmd.ExecuteNonQueryAsync();
        //}
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