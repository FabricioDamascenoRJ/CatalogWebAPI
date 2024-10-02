using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class PopularCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categories(Name, ImageUrl) Values('Bebidas', 'bebidas.jpg')");
            mb.Sql("Insert into Categories(Name, ImageUrl) Values('Lanches', 'lanches.jpg')");
            mb.Sql("Insert into Categories(Name, ImageUrl) Values('Sobremesas', 'sobremesas.jpg')");


            //Inseri dados na tabela de produto após criar as categorias.
            mb.Sql("Insert into Products(Name, Description, Price, ImageUrl, Stock, CreatedAt, CategoryId) " +
                "Values('Coca-cola Diet', 'Refrigerante lata de 350ml', 5.45, 'cocacola.jpg', 50, now(), 1)");

            mb.Sql("Insert into Products(Name, Description, Price, ImageUrl, Stock, CreatedAt, CategoryId) " +
                 "Values('Lanche de Atum','Lanche de Atum com maionese',8.50,'atum.jpg',10,now(),2)");

            mb.Sql("Insert into Products(Name, Description, Price, ImageUrl, Stock, CreatedAt, CategoryId) " +
                "Values('Pudim 100 g','Pudim de leite condensado 100g',6.75,'pudim.jpg',20,now(),3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categories");
            mb.Sql("Delete from Products");
        }
    }
}
