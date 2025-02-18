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

    async getUserCharacters(id){
        return await this.UserCharacter.findAll(
            {
                where:{
                    user_id:id
                },
                attributes: ['lvl','attack_power'],
                include: [
                    {
                        model: this.Character,
                        attributes: ['name','description']
                    }
                ]
            }
        )
    }
}

module.exports = new userRepository(db)