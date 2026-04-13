from bot.user_base import USERS
from bot.texts import TEXTS
def extract_user_data(update, lang: str) -> dict:
    user = update.effective_user

    return {
        "telegram_id": user.id,
        "username": user.username,
        "selected_bot_language": lang,
    }

def t(lang: str, key: str) -> str:
    return TEXTS.get(lang, TEXTS["ru"]).get(key, key)


