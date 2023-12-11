SELECT 
DF_id AS DrinkFoodID,
DF_menu_id AS MenuID,
DF_name AS DrinkFoodName,
CT_order AS DrinkFoodTypeOrder,
CT_id AS DrinkFoodTypeID,
CT_desc AS DrinkFoodTypeDesc,
DF_price AS DrinkFoodPrice,
ISNULL(DF_remark, '') AS DrinkFoodRemark
FROM DrinkFood
JOIN CodeTable ON DF_type = CT_id
WHERE DF_status != '99' AND CT_type = 'DrinkFoodType';