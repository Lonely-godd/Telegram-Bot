from telegram.ext import ApplicationBuilder, CommandHandler, CallbackQueryHandler

from bot.config import BOT_TOKEN
from bot.handlers.start import start_command
from bot.handlers.faq import menu_callback


def main() -> None:
    app = ApplicationBuilder().token(BOT_TOKEN).build()

    app.add_handler(CommandHandler("start", start_command))
    app.add_handler(CallbackQueryHandler(menu_callback))

    print("Bot is running...")
    app.run_polling()


if __name__ == "__main__":
    main()