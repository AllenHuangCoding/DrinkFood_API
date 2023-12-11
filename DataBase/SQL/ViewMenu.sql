SELECT
M_id AS MenuID,
M_area_id AS MenuAreaID,
CT_desc AS MenuAreaDesc,
M_create AS MenuCreate,
B_id AS BrandID,
B_name AS BrandName
FROM Menu 
JOIN Brand ON M_brand_id = B_id
JOIN CodeTable ON M_area_id = CT_id
WHERE menu.M_status != '99' AND brand.B_status != '99' AND CT_type = 'MenuArea';