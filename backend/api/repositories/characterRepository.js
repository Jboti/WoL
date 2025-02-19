const db = require('../db/dbContext')

class characterRepository
{
    constructor(db) {
        this.Character = db.character
    }

    async getAllCharacters() {
        return await this.Character.findAll({})
    }
}

module.exports = new characterRepository(db)
