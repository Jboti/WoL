const userService = require('../services/userService')
const bcrypt = require('bcryptjs')

exports.registerUser = async (req,res,next) => {
    const { username, password } = req.body
    if (!username || !password)
    {
        const error = new Error("Missing inputs!")
        error.status = 400
        throw error
    }
    try {
        const hashedPassword = await bcrypt.hash(password, 10)
        const result = userService.createUser(username,hashedPassword)
        if(!result)
            res.status(500).json({ error: 'Hiba a regisztráció során!' })
        res.status(201).json({ message: 'Sikeres regisztráció!' })
    } catch (err) {
        res.status(500).json({ error: 'Hiba a regisztráció során!' })
    }
}

exports.loginUser = async (req,res,next) => {
    const { username, password } = req.body
    if (!username || !password)
    {
        const error = new Error("Missing inputs!")
        error.status = 400
        throw error
    }
    try {
        const user = await userService.getUserFromUsername(username)
        if(!user || !(await bcrypt.compare(password,user.password)))
            res.status(401).json({ error: 'Hiba a bejelentkezés során!'})

        res.status(200).json({ message: 'Sikeres bejelentkezés!' })
       
    } catch (err) {
        res.status(500).json({ error: 'Hiba a bejelentkezés során!' })
    }
}