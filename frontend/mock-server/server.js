const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors');

const app = express();
const port = 8000;

app.use(bodyParser.json());
app.use(cors());
app.use(express.urlencoded({extended: true}));

// Запустить мок-сервер:
// npm i
// nodemon server.js

const PARAMS = {
    'technologies': [
        {value: 'ar', label: 'AR/VR'},
        {value: 'neurotechnology', label: 'Нейротехнологии'},
        {value: 'biotechnology', label: 'Биотехнологии'},
        {value: '3d', label: '3D моделирование'},
        {value: 'drones', label: 'Беспилотники'}
    ],
    'legal': [
        {value: 'micro', label: 'ЮЛ Микро'},
        {value: 'avg', label: 'ЮЛ Среднее'}
    ],
    'stage': [
        {value: 'mature', label: 'Зрелость'},
        {value: 'extension', label: 'Расширение'},
        {value: 'any', label: 'Любая'}
    ],
    'market': [
        {value: 'healthcare', label: 'Healthcare'},
        {value: 'safety-tech', label: 'SafetyTech'},
        {value: 'retail-tech', label: 'RetailTech'}
    ],
    'department': [
        {value: 'hr-tech', label: 'HRTech'},
        {value: 'ed-tech', label: 'EdTech'},
        {value: 'fin-tech', label: 'FinTech'}
    ]
};

const RECOMMENDATIONS = [
    {id: 'item-1', title: 'PropTech', score: 15},
    {id: 'item-2', title: 'IndustrialTech', score: 12},
    {id: 'item-3', title: 'Business Software', score: 10},
    {id: 'item-4', title: 'Healthcare', score: 10},
    {id: 'item-5', title: 'Transport & Logistics', score: 8}
];

app.get('/params', (req, res) => {
    res.send({
        params: PARAMS
    });
});

app.post('/save', (req, res) => {
    res.send({
        recommendations: RECOMMENDATIONS
    });
});

app.post('/helped-me', (req, res) => {
    res.send({
        message: 'ok'
    });
});

app.listen(port);
