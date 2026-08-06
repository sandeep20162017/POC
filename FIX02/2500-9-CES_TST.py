Now we have connected sucessfully to databases. Can we run simple queries like getiing names starting with 'SCPart%' on bothoracleand ms sql.
give meupdated script. Then we will extend ittocopy data fromoracle  to ms sql

import platform
import sys
import pyodbc
import oracledb


print("=" * 70)
print("PYTHON INFORMATION")
print("=" * 70)

print("Python :", sys.version)
print("Arch   :", platform.architecture())


############################################################
# Oracle Test
############################################################

print("\n" + "=" * 70)
print("ORACLE CONNECTION TEST")
print("=" * 70)

try:
    oracle_conn = oracledb.connect(
    user="CES",
    password="ces",
    dsn="//ocinp-f453r-scan.ttcocidrclients.ttcocidrvcn.oraclevcn.com:1521/ces_upg_svc.int.ttc.ca"
)
    print("Oracle connection SUCCESS")

    cursor = oracle_conn.cursor()

    cursor.execute(
        """
        SELECT 
            SYS_CONTEXT('USERENV','DB_NAME'),
            SYSDATE
        FROM DUAL
        """
    )

    result = cursor.fetchone()

    print("Database :", result[0])
    print("Date     :", result[1])

    cursor.close()
    oracle_conn.close()


except Exception as e:

    print("Oracle connection FAILED")
    print(type(e).__name__)
    print(e)



############################################################
# SQL SERVER Test
############################################################


print("\n" + "=" * 70)
print("SQL SERVER CONNECTION TEST")
print("=" * 70)


sql_connection_string = (
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=AppCESDB_DEV;"
    "DATABASE=CES;"
    "UID=CESUser;"
    "PWD=CESUser#Dev;"
    "TrustServerCertificate=Yes;"
)


try:

    sql_conn = pyodbc.connect(
        sql_connection_string,
        timeout=10
    )

    print("SQL Server connection SUCCESS")

    cursor = sql_conn.cursor()

    cursor.execute(
        """
        SELECT 
            @@SERVERNAME,
            DB_NAME(),
            GETDATE()
        """
    )

    result = cursor.fetchone()

    print("Server :", result[0])
    print("DB     :", result[1])
    print("Date   :", result[2])


    cursor.close()
    sql_conn.close()


except Exception as e:

    print("SQL Server connection FAILED")
    print(type(e).__name__)
    print(e)



print("\nTest completed.")
===============
logs
(python31064bit) C:\ces>python ces_test.py
======================================================================
PYTHON INFORMATION
======================================================================
Python : 3.10.3 (tags/v3.10.3:a342a49, Mar 16 2022, 13:07:40) [MSC v.1929 64 bit (AMD64)]
Arch   : ('64bit', 'WindowsPE')

======================================================================
ORACLE CONNECTION TEST
======================================================================
Oracle connection SUCCESS
Database : CES_UPG
Date     : 2026-08-06 18:22:09

======================================================================
SQL SERVER CONNECTION TEST
======================================================================
SQL Server connection SUCCESS
Server : D0814W001
DB     : CES
Date   : 2026-08-06 18:22:09.553000

Test completed.
