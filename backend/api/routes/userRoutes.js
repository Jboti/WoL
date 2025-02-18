const express = require('express')
const router = express.Router()

const userController = require('../controllers/userController')
const authMiddleware = require('../middlewares/auth')

router.post("/register", authMiddleware.APIKeyValidator, userController.registerUser)
router.post("/login", authMiddleware.APIKeyValidator, userController.loginUser)
router.get("/user-characters",authMiddleware.APIKeyValidator, userController.getUserCharacters)


module.exports = router