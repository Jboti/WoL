module.exports = (sequelize,DataTypes) => {
    const User = require('./User')(sequelize,DataTypes)
    const Character = require('./Character')(sequelize,DataTypes)
    const UserCharacter = require('./UserCharacter')(sequelize,DataTypes)

    // User -> UserCharacter (egy felhasználónak több karaktere lehet)
    User.hasMany(UserCharacter, { foreignKey: 'user_id' });
    UserCharacter.belongsTo(User, { foreignKey: 'user_id' });

    // Character -> UserCharacter (egy karaktertömb több user karaktert is tartalmazhat)
    Character.hasMany(UserCharacter, { foreignKey: 'character_id' });
    UserCharacter.belongsTo(Character, { foreignKey: 'character_id' });


    return { User, Character, UserCharacter }
}