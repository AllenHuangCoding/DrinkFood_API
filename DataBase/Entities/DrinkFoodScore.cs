using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("DrinkFoodScore")]
public partial class DrinkFoodScore
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid DFS_id { get; set; }

    public Guid DFS_drink_food_id { get; set; }

    public Guid DFS_order_detail_id { get; set; }

    public DateTime DFS_create { get; set; }

    public DateTime DFS_update { get; set; }
}
