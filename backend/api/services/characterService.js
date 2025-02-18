const characterRepository = require('../repositories/characterRepository')

class characterService
{
    async getAllCharacters()
    {
        return await characterRepository.getAllCharacters()
    }
}

module.exports = new characterService()