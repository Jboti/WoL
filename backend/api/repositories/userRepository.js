const db = require('../db/dbContext')

class userRepository{
    constructor(db){
        this.User = db.user
        this.UserCharacter = db.userCharacter
        this.Character = db.character
    }

    async createUser(user){
        const newUser = await this.User.create(user)
        await newUser.save()
        return newUser
    }

    async getUserFromUsername(username){
        return await this.User.findOne(
            {
                where:{
                    username:username
                }
            }
        )
    }

    async getUserCharacters(id) {
        return await this.UserCharacter.findAll({
            where: { user_id: id },
            attributes: ['id','name', 'lvl', 'attack_power'],
            include: [
                {
                    model: this.Character,
                    attributes: ['characterName', 'description']
                }
            ]
        })
    }
    
    async createNewCharacter(user_character){
        const newCharacter = await this.UserCharacter.create(user_character)
        await newCharacter.save()
        return newCharacter
    }

}

module.exports = new userRepository(db)