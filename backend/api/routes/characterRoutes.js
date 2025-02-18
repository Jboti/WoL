const express = require('express')
const router = express.Router()

//ROUTES
const characterController = require('../controllers/characterController')
const authMiddleware = require('../middlewares/auth')

router.get("/get-characters", authMiddleware.APIKeyValidator, characterController.getAllCharacters)


module.exports = router