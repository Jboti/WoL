const userRepository = require('../repositories/userRepository')

class userService
{
    async createUser(username,hashedpw){
        const user = {
            id: null,
            username:username,
            password:hashedpw
        }
        return await userRepository.createUser(user)
    }

    async getUserFromUsername(username){
        return await userRepository.getUserFromUsername(username)
    }
}

module.exports = new userService()