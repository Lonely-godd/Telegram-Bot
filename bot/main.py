from telegram.ext import (
    ApplicationBuilder,
    CallbackQueryHandler,
    CommandHandler,
    ConversationHandler,
    MessageHandler,
    filters,
)

from bot.config import BOT_TOKEN
from bot.handlers.start import start_command
from bot.handlers.faq import menu_callback
from bot.handlers.question import (
    WAITING_FOR_QUESTION,
    ask_volunteer_start,
    receive_question,
    cancel_question,
)


def main() -> None:
    app = ApplicationBuilder().token(BOT_TOKEN).build()

    question_conversation = ConversationHandler(
        entry_points=[CallbackQueryHandler(ask_volunteer_start, pattern="^ask_volunteer$")],
        states={
            WAITING_FOR_QUESTION: [
                MessageHandler(filters.TEXT & ~filters.COMMAND, receive_question),
            ],
        },
        fallbacks=[CommandHandler("cancel", cancel_question)],
    )

    app.add_handler(CommandHandler("start", start_command))
    app.add_handler(question_conversation)
    app.add_handler(CallbackQueryHandler(menu_callback))

    print("Bot is running...")
    app.run_polling()


if __name__ == "__main__":
    main()