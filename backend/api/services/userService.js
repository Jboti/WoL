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

    async getUserCharacters(id)
    {
        return await userRepository.getUserCharacters(id)
    }

    async createNewCharacter(uid,cid,name)
    {
        const character = {
            id: null,
            user_id: uid,
            character_id: cid,
            name: name,
        }
        return await userRepository.createNewCharacter(character)
    }

}

module.exports = new userService()