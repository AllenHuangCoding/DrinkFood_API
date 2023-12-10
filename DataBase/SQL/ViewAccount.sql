SELECT
account.A_id AS AccountID,
account.A_name AS Name,
account.A_brief AS Brief,
account.A_email AS Email,
account.A_line_id AS LineID,
account.A_line_notify AS LineNotify,
account.A_lunch_notify AS LunchNotify,
account.A_drink_notify AS DrinkNotify,
account.A_close_notify AS CloseNotify,
drinkPayment.CT_id AS DefaultDrinkPayment,
drinkPayment.CT_desc AS DefaultDrinkPaymentDesc,
lunchPayment.CT_id AS DefaultLunchPayment,
lunchPayment.CT_desc AS DefaultLunchPaymentDesc,
account.A_is_admin AS IsAdmin
FROM Account 
LEFT JOIN CodeTable AS drinkPayment ON A_default_drink_payment = drinkPayment.CT_id
LEFT JOIN CodeTable AS lunchPayment ON A_default_lunch_payment = lunchPayment.CT_id
WHERE A_status != '99'

