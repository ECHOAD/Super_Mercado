using Microsoft.EntityFrameworkCore.Migrations;

namespace Super_Mercado.Migrations
{
    public partial class STOREPROCEDURE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            var sp = @"CREATE PROCEDURE SP_Orden 
            (
                @User NVARCHAR(30),
                @Fecha DATETIME = NULL,
                @Total FLOAT,
                @Estado INT =0
            )
            AS

                DECLARE @ID_USUARIO INT

                SET @Fecha= CURRENT_TIMESTAMP

                SELECT @ID_USUARIO= Id FROM Usuarios WHERE [User] = @User

                INSERT INTO Ordenes (Id_Usuario,Fecha,Total,Estado) VALUES (@ID_USUARIO, @Fecha,@Total,@Estado)";

            migrationBuilder.Sql(sp);


            sp = @"CREATE PROCEDURE SPUsuarioDireciones
            (
            @User NVARCHAR(30),
            @Dirrecion NVARCHAR(100),
            @Comentario NVARCHAR(100),
            @Latitud DECIMAL(18,2),
            @Longitud DECIMAL(18,2)
            )
            AS
            DECLARE @ID_USUARIO INT
            
            SELECT @ID_USUARIO= Id FROM Usuarios WHERE [User] = @User
            
            iF(@ID_USUARIO IS NOT NULL)
            INSERT INTO UsuariosDirecciones (Id_Usuario,Direccion,Comentario,Latitud,Longitud)
            VALUES(@ID_USUARIO,@Dirrecion,@Comentario,@Latitud,@Longitud)";

            migrationBuilder.Sql(sp);


            sp = @"CREATE PROCEDURE SPOrdenDetalle
            (
            @Id_Orden INT,
            @Id_Producto INT,
            @Cantidad INT
            )
            AS

            INSERT INTO Ordenes_Detalles(Id_orden,Id_Producto,Cantidad)
            VALUES(@Id_Orden,@Id_Producto,@Cantidad)";


            migrationBuilder.Sql(sp);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE SP_Orden";

            migrationBuilder.Sql(sp);

            sp = @"DROP PROCEDURE SPUsuarioDireciones";

            migrationBuilder.Sql(sp);

            sp = @"DROP PROCEDURE SPOrdenDetalle";

            migrationBuilder.Sql(sp);


        }
    }
}
