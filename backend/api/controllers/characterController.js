const characterService = require('../services/characterService')

exports.getAllCharaters = async (req,res,next) => {
    try {
        const characters = await characterService.getAllCharacters()
        res.status(200).json(characters)
    } catch (err) {
        next(err)
    }
}