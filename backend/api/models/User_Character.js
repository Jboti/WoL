module.exports = (sequelize,DataTypes) =>{
    const User_Character = sequelize.define(
        "user_character",
        {
            id:{
                type: DataTypes.INTEGER,
                autoIncrement: true,
                primaryKey: true,
            },
            lvl:{
                type: DataTypes.INTEGER,
                defaultValue: 1,
            },
            attack_power:{
                type:DataTypes.INTEGER,
                defaultValue: 5,
            }
        },
        {
            timestamps: false
        }
    )

    return User_Character
}