SELECT
    OD_order_id AS OrderID,
    OD_id AS OrderDetailID,
    OD_drink_food_id AS DrinkFoodID,
    DF_name AS DrinkFoodName,
    DF_price AS DrinkFoodPrice,
    ISNULL(DF_remark, '') AS DrinkFoodRemark,
    OD_sugar_id AS SugarID,
    sugarOption.O_name AS SugarDesc,
    OD_ice_id AS IceID,
    iceOption.O_name AS IceDesc,
    OD_size_id AS SizeID,
    sizeOption.O_name AS SizeDesc,
    OD_account_id AS DetailAccountID,
    A_name AS Name,
    A_brief AS Brief,
    A_email AS Email,
    OD_payment_id AS PaymentID,
    paymentCodeTable.CT_desc AS PaymentDesc,
    OD_payment_datetime AS PaymentDatetime,
    OD_quantity AS Quantity,
    OD_pickup AS IsPickup,
    OD_remark AS DetailRemark,
    [order].O_status AS OrderStatus,
    [order].O_arrival_time AS ArrivalTime,
    [order].O_close_time AS CloseTime,
    [order].O_create_account_id AS OwnerID,
	brand.B_name AS BrandName
FROM OrderDetail
JOIN [Order] [order] ON OD_order_id = [order].O_id
JOIN Office office ON [order].O_office_id = office.O_id
JOIN Store store ON [order].O_store_id = store.S_id
JOIN Brand brand ON store.S_brand_id = brand.B_id
JOIN Account account ON OD_account_id = A_id
LEFT JOIN DrinkFood ON OD_drink_food_id = DF_id
LEFT JOIN [Option] sugarOption ON OD_sugar_id = sugarOption.O_id
LEFT JOIN [Option] iceOption ON OD_ice_id = iceOption.O_id
LEFT JOIN [Option] sizeOption ON OD_size_id = sizeOption.O_id
LEFT JOIN CodeTable paymentCodeTable ON OD_payment_id = paymentCodeTable.CT_id AND paymentCodeTable.CT_type LIKE '%Payment%'