const db = require('../db/dbContext')

class userRepository{
    constructor(db){
        this.User = db.user
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
}

module.exports = new userRepository(db)