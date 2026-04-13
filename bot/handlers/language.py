from telegram import Update, InlineKeyboardMarkup
from telegram.ext import ContextTypes
from texts import TEXTS
from bot.keyboards.menu import get_main_menu_keyboard, get_language_keyboard
from bot.helpers import extract_user_data, t
from bot.db import upsert_user


async def language_callback(update: Update, context: ContextTypes.DEFAULT_TYPE):
    query = update.callback_query
    await query.answer()
    data = query.data

    if data == "set_lang_ru":
        lang = "ru"
    elif data == "set_lang_ua":
        lang = "ua"
    else:
        return
    user_data = extract_user_data(update, lang)
    upsert_user(user_data)

    await query.edit_message_text(
        text=t(lang, "start_message"),
        reply_markup=get_main_menu_keyboard(),
    )
