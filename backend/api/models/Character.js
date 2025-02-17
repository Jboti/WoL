module.exports = (sequelize, DataTypes) =>{
    const Character = sequelize.define(
        "character",
        {
            id: {
                type: DataTypes.TINYINT,
                autoIncrement: true,
                primaryKey: true,
            },
            name:{
                type: DataTypes.STRING,
                unique: true,
                allowNull: false,
            },
            description:{
                type: DataTypes.TEXT,
                allowNull: false,
            }
        },
        {
            timestamps: false,
        }
    )

    return Character
}