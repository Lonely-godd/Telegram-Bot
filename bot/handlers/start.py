from telegram import Update, InlineKeyboardMarkup
from telegram.ext import ContextTypes
from texts import TEXTS
from bot.keyboards.menu import get_main_menu_keyboard, get_language_keyboard
from bot.helpers import extract_user_data, t
from bot.user_base import USERS


async def start_command(update: Update, context: ContextTypes.DEFAULT_TYPE) -> None:
    print(extract_user_data(update))
    user_data = extract_user_data(update)
    await update.message.reply_text(
        text = "Выберите язык / Оберіть мову:",
        reply_markup = get_language_keyboard(),
    )
    return
