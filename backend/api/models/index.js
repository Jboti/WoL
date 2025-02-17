module.exports = (sequelize,DataTypes) => {
    const User = require('./User')(sequelize,DataTypes)
    const Character = require('./Character')(sequelize,DataTypes)
    const User_Character = require('./User_Character')(sequelize,DataTypes)

    User.belongsToMany(Character,{
        through:User_Character,
        foreignKey:'user_id',
        otherKey:'character_id'
    })
    Character.belongsToMany(User,{
        through:User_Character,
        foreignKey:'character_id',
        otherKey:'user_id'
})

    return { User, Character, User_Character }
}