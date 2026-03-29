from bot.user_base import USERS
from bot.texts import TEXTS
def extract_user_data(update):
    user = update.effective_user
    chat = update.effective_chat

    if user.id not in USERS:
        USERS[user.id] = {
            "username": user.username,
            "selected_bot_language": None,
        }

    return {
        "chat_id": chat.id,
        "telegram_id": user.id,
        "username": user.username,
        "selected_bot_language": USERS[user.id]["selected_bot_language"],
    }

def t(lang: str, key: str) -> str:
    return TEXTS.get(lang, TEXTS["ru"]).get(key, key)


