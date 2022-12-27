export const currencyFormat = (number: any) => {
    return '$' + number.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
};