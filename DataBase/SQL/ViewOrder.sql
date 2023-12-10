SELECT
    [Order].O_id AS OrderID,
    O_create_account_id AS OwnerID,
    A_name AS OwnerName,
    A_brief AS OwnerBrief,
    O_no AS OrderNo,
    O_type AS Type,
    typeCodeTable.CT_desc AS TypeDesc,
    O_is_public AS IsPublic,
    O_share_url AS ShareUrl,
    O_arrival_time AS ArrivalTime,
    O_open_time AS OpenTime,
    O_close_time AS CloseTime,
    O_close_remind_time AS CloseRemindTime,
    O_remark AS Remark,
    [Order].O_status AS OrderStatus,
    '尚未設定' AS OrderStatusDesc,  -- 需要根據實際情況調整
    [Order].O_create AS CreateTime,
    O_office_id AS OfficeID,
    O_name AS OfficeName,
    B_id AS BrandID,
    B_name AS BrandName,
    B_logo AS BrandLogoUrl,
    B_official_url AS BrandOfficialUrl,
    O_store_id AS StoreID,
    S_name AS StoreName,
    S_address AS StoreAddress,
    S_phone AS StorePhone
FROM [Order]
JOIN Store store ON O_store_id = S_id
JOIN Brand brand ON S_brand_id = B_id
JOIN Account account ON O_create_account_id = A_id
JOIN Office office ON O_office_id = Office.O_id
JOIN CodeTable typeCodeTable ON O_type = typeCodeTable.CT_id
WHERE [Order].O_status != '99' AND S_status != '99' AND A_status != '99' AND Office.O_status != '99' AND typeCodeTable.CT_type = 'OrderType';