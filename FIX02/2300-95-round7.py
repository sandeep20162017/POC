# ============================
# ces_round7.py (MBList + RbMasterlist)
# ============================

import sys
import logging
import pyodbc
import oracledb

ORACLE_DSN ="//ocipr-wersc-scan.ttcociclientsub.ttcocivcn.oraclevcn.com:1521/ces_prd_svc.int.ttc.ca"
ORACLE_USER = "CES"
ORACLE_PASSWORD = "cesprd1210"

SQL_CONN_STR = (
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=SQL2022UAT;"
    "DATABASE=CES;"
    "UID=cesuser;"
    "PWD=CESUser#UAT;"
    "TrustServerCertificate=Yes;"
)

SQL_SCHEMA = "SBCES"

logger = logging.getLogger("migration_round7")
logger.setLevel(logging.INFO)
handler = logging.FileHandler("migration_round7.log", mode="a", encoding="utf-8")
console = logging.StreamHandler(sys.stdout)
fmt = logging.Formatter("%(asctime)s [%(levelname)s] %(message)s")
handler.setFormatter(fmt)
console.setFormatter(fmt)
logger.addHandler(handler)
logger.addHandler(console)

def get_oracle_conn():
    return oracledb.connect(user=ORACLE_USER, password=ORACLE_PASSWORD, dsn=ORACLE_DSN)

def get_sql_conn():
    conn = pyodbc.connect(SQL_CONN_STR, timeout=10)
    cur = conn.cursor()
    cur.execute("SET ANSI_WARNINGS OFF;")
    cur.close()
    return conn

def delete_sql_table(sql_conn, table):
    cur = sql_conn.cursor()
    try:
        cur.execute(f"DELETE FROM {SQL_SCHEMA}.{table}")
        sql_conn.commit()
        logger.info(f"Deleted {table}")
    except Exception as e:
        logger.error(f"Delete failed for {table}: {e}")
    cur.close()

def fetch_oracle_rows(oracle_conn, sql):
    cur = oracle_conn.cursor()
    cur.execute(sql)
    rows = cur.fetchall()
    cur.close()
    return rows

# ----------------------------------------
# MBList Migration (explicit mapping)
# ----------------------------------------

def migrate_mblist(oracle_conn, sql_conn):
    logger.info("Migrating MbList (explicit mapping)")

    rows = fetch_oracle_rows(oracle_conn, """
        SELECT
            MB_NUMBER,
            KEYWORD,
            DESCRIPTION,
            BUS_TYPE,
            NUM_BUS_COMP,
            VENDOR_PART_NUM,
            BUY_CODE,
            REBUILD_CODE,
            CORE_CODE,
            ANNUAL_USAGE,
            BENCH_NUMBER,
            SAMPLE_PROVIDED,
            RECOMMENDATION,
            REMAN_COST,
            EXTERNAL_COST,
            BUY_NEW_COST,
            INTERNAL_COST,
            SOP_NUMBER,
            CORE_COST,
            JOB_NUMBER,
            DATE_OPENED,
            READMANUALNEWPRICE
        FROM MB_LIST
    """)

    sql = f"""
        INSERT INTO {SQL_SCHEMA}.MbList (
            MbNumber, Keyword, Description, BusType, NumBusComp,
            VendorPartNum, BuyCode, RebuildCode, CoreCode, AnnualUsage,
            BenchNumber, SampleProvided, Recommendation, RemanCost,
            ExternalCost, BuyNewCost, InternalCost, SopNumber, CoreCost,
            JobNumber, DateOpened, Readmanualnewprice, MbPrefix
        ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, NULL)
    """

    cur = sql_conn.cursor()
    success = 0
    fail = 0

    for idx, r in enumerate(rows):
        try:
            cur.execute(sql, r)
            success += 1
        except Exception as e:
            fail += 1
            logger.error(f"MbList row {idx} failed: {e} | Row={r}")

    sql_conn.commit()
    cur.close()
    logger.info(f"MbList: {success} succeeded, {fail} failed")

# ----------------------------------------
# RbMasterlist Migration (explicit mapping)
# ----------------------------------------

def migrate_rbmasterlist(oracle_conn, sql_conn):
    logger.info("Migrating RbMasterlist (explicit mapping)")

    rows = fetch_oracle_rows(oracle_conn, """
        SELECT
            MMS_STOCK_CODE,
            ITEM_REF_NUMBER,
            DETAILED_DESC,
            KEYWORD,
            CORE_PART_NUM,
            REBUILT_STOCK_NUM,
            CORE_CHARGE,
            JOB_NUMBER,
            DATE_MODIFIED,
            LAST_MODIFIED_BY,
            ESTIMATED_COST,
            SOP_NUMBER,
            BUY_NEW_COST,
            REMAN_COST,
            EXTERNAL_COST,
            DATE_CONVERTED,
            ACTIVE
        FROM RB_MASTERLIST
    """)

    sql = f"""
        INSERT INTO {SQL_SCHEMA}.RbMasterlist (
            MmsStockCode, ItemRefNumber, DetailedDesc, Keyword, CorePartNum,
            RebuiltStockNum, CoreCharge, JobNumber, DateEntered, LastModifiedBy,
            EstimatedCost, SopNumber, BuyNewCost, RemanCost, ExternalCost,
            Active
        ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
    """

    cur = sql_conn.cursor()
    success = 0
    fail = 0

    for idx, r in enumerate(rows):
        (
            mms_stock_code,
            item_ref_number,
            detailed_desc,
            keyword,
            core_part_num,
            rebuilt_stock_num,
            core_charge,
            job_number,
            date_modified,
            last_modified_by,
            estimated_cost,
            sop_number,
            buy_new_cost,
            reman_cost,
            external_cost,
            date_converted,
            active
        ) = r

        try:
            cur.execute(sql, (
                mms_stock_code,
                item_ref_number,
                detailed_desc,
                keyword,
                core_part_num,
                rebuilt_stock_num,
                core_charge,
                job_number,
                date_modified,
                last_modified_by,
                estimated_cost,
                sop_number,
                buy_new_cost,
                reman_cost,
                external_cost,
                active
            ))
            success += 1
        except Exception as e:
            fail += 1
            logger.error(f"RbMasterlist row {idx} failed: {e} | Row={r}")

    sql_conn.commit()
    cur.close()
    logger.info(f"RbMasterlist: {success} succeeded, {fail} failed")

# ----------------------------------------
# MAIN
# ----------------------------------------

def main():
    oracle_conn = get_oracle_conn()
    sql_conn = get_sql_conn()

    delete_sql_table(sql_conn, "MbList")
    delete_sql_table(sql_conn, "RbMasterlist")

    migrate_mblist(oracle_conn, sql_conn)
    migrate_rbmasterlist(oracle_conn, sql_conn)

    oracle_conn.close()
    sql_conn.close()

if __name__ == "__main__":
    main()
