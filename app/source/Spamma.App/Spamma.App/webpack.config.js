const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: {
        app: ['./Assets/styles/site.scss'],
    },
    devtool: "inline-source-map",
    output: {
        path: path.join(__dirname, 'wwwroot/'),
        publicPath: '/',
        filename: '[name].js'
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: '[name].css',
            chunkFilename: '[id].css',
            ignoreOrder: false
        }),
    ],
    module: {
        rules: [
            {
                test: /\.(css|sass|scss)$/,
                use: [MiniCssExtractPlugin.loader, 'css-loader', 'sass-loader']
                //☝🏽 Load Sass files
            },
            {
                // To use images on pug files:
                test: /\.(png|jpg|jpeg|ico)/,
                type: 'asset/resource',
                generator: {
                    filename: '[name][ext]'
                }
            },
            {
                // To use fonts on pug files:
                test: /\.(woff|woff2|eot|ttf|otf|svg)$/i,
                type: 'asset/resource',
                generator: {
                    filename: '[name][ext][query]'
                }
            },
            {
                // To use config:
                test: /\.(json)$/i,
                type: 'asset/resource',
                generator: {
                    filename: '[name][ext]'
                }
            },
            {
                test: /\.ts?$/,
                use: 'ts-loader',
                exclude: /node_modules/,
                generator: {
                    filename: 'abc.js'
                }
            },
        ]
    },
    stats: 'errors-only',
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
    }
    //☝🏽 For a cleaner dev-server run
};