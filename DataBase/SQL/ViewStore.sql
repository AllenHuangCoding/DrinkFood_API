SELECT
B_id AS B_id,
B_name AS B_name,
B_logo AS B_logo,
B_line_id AS B_line_id,
B_official_url AS B_official_url,
codeTable.CT_desc AS B_type_desc,
S_id AS S_id,
S_brand_id AS S_brand_id,
S_name AS S_name,
S_address AS S_address,
S_line_id AS S_line_id,
S_menu_area_id AS S_menu_area_id,
S_phone AS S_phone,
S_remark AS S_remark
FROM Store
JOIN Brand ON S_brand_id = B_id
JOIN CodeTable ON B_type = CT_id
WHERE S_status != '99' AND B_status != '99' AND codeTable.CT_type = 'BrandType';