SELECT
O_id AS O_id,
O_name AS O_name,
O_address AS O_Address,
O_company_id AS O_company_id,
C_name AS C_name
FROM Office 
JOIN Company ON O_company_id = C_id
WHERE O_status != '99' AND C_status != '99';