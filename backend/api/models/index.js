module.exports = (sequelize,DataTypes) => {
    const User = require('./User')(sequelize,DataTypes)
    const Character = require('./Character')(sequelize,DataTypes)
    const UserCharacter = require('./UserCharacter')(sequelize,DataTypes)

    User.belongsToMany(Character,{
        through:UserCharacter,
        foreignKey:'user_id',
        otherKey:'character_id'
    })
    Character.belongsToMany(User,{
        through:UserCharacter,
        foreignKey:'character_id',
        otherKey:'user_id'
})

    return { User, Character, UserCharacter }
}