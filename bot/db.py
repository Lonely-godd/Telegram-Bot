import sqlite3
from datetime import datetime
DB_NAME = 'bot.db'


def get_connection():
    return sqlite3.connect(DB_NAME)


def init_db():
    conn = get_connection()
    cursor = conn.cursor()

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS users (
        telegram_id INTEGER NOT NULL PRIMARY KEY,
        username TEXT,
        selected_bot_language TEXT CHECK (selected_bot_language IN ('ru', 'ua')),
        created_at TEXT,
        last_seen_at TEXT
        )
    """)
    conn.commit()
    conn.close()


def upsert_user(user_data: dict):
    conn = get_connection()
    cursor = conn.cursor()

    now = datetime.utcnow().isoformat()
    cursor.execute("""
        INSERT INTO users (telegram_id, username, selected_bot_language, created_at, last_seen_at)
        VALUES (?, ?, ?, ?, ?)
        ON CONFLICT (telegram_id) DO UPDATE SET
            username = excluded.username,
            selected_bot_language = excluded.selected_bot_language,
            last_seen_at = excluded.last_seen_at""",
                   (
                       user_data['telegram_id'],
                       user_data['username'],
                       user_data['selected_bot_language'],
                       now,
                       now,
                   ))
    conn.commit()
    conn.close()


def set_user_language(telegram_id: int, language: str):
    conn = get_connection()
    cursor = conn.cursor()

    cursor.execute("""
        UPDATE users
        SET selected_bot_language = ? 
        WHERE telegram_id = ?
    """, (language, telegram_id))
    conn.commit()
    conn.close()

def check():
    conn = sqlite3.connect("bot.db")
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM users;")
    print(cursor.fetchall())
    conn.close()
