import os
from dotenv import load_dotenv

load_dotenv()

BOT_TOKEN = os.getenv("BOT_TOKEN")
ADMIN_CHAT_ID = os.getenv("ADMIN_CHAT_ID")

if not BOT_TOKEN:
    raise ValueError("BOT_TOKEN not found in .env")

if not ADMIN_CHAT_ID:
    raise ValueError("ADMIN_CHAT_ID not found in .env")

ADMIN_CHAT_ID = int(ADMIN_CHAT_ID)