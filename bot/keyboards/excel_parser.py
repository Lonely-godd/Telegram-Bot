import gspread

client = gspread.service_account("service_account.json")
sheet = client.open("tgbot").sheet1
print(sheet.acell("A799").value)