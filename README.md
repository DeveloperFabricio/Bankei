# Bankei

## Escopo

Desenvolver uma API que faça a criação e gerenciamento de investimentos, contendo as funcionalidades abaixo:

1. **Criação de um investimento**
    - Endpoint contendo uma data e o valor a ser investido.
    
    - O investimento pode ser na data de hoje ou em uma data do passado, mas não em uma data do futuro.
    
    - O valor do investimento não poderá ser negativo.
    

2. **Visualização de um investimento**
  
    - Deverá retornar o valor inicial do investimento e o saldo esperado para determinada data.
    
    - A data será um campo opcional, e caso não seja informada, deverá ser considerado a data atual.
    
    - Caso receba uma data, ela não pode ser inferior a data do investimento.
    
    - O saldo esperado deverá ser a soma do valor investido e dos ganhos até a data (regra dos ganhos a ser explicada abaixo)
    
  
3. **Saque de um investimento**
    - O valor do saque deverá ser a soma do valor investido e dos ganhos obtidos até a data do saque.
    
    - Não deverá permitir saques parciais ou maiores do que o valor disponível.
    
    - Os saques só podem ser realizados na data atual.
    
    - O saque só poderá ocorrer uma vez para cada investimento.
    

### Cálculo de ganhos

O investimento irá pagar 1.16% todos os meses, sempre no mesmo dia do mês em que o investimento foi criado.

Dado que o investimento é pago mensalmente, deverá ser aplicado uma regra de juros compostos, ou seja, o valor ganho nos meses anteriores deverá ser considerado no cálculo do mês atual, fazendo com que o ganho aumente a cada mês.

Exemplo:

| Mês | Total Investido | Ganho do Mês | Total Ganhos | Saldo esperado |
| --- | --- | --- | --- | --- |
| 0   | R$ 1.000,00 | R$ 0,00 | R$ 0,00 | R$ 1.000,00 |
| 1   | R$ 1.000,00 | R$ 11,60 | R$ 11,60 | R$ 1.011,60 |
| 2   | R$ 1.000,00 | R$ 11,73 | R$ 23,33 | R$ 1.023,33 |
| 3   | R$ 1.000,00 | R$ 11,87 | R$ 35,21 | R$ 1.035,21 |
| 4   | R$ 1.000,00 | R$ 12,01 | R$ 47,21 | R$ 1.047,21 |

![Testes](https://github.com/DeveloperFabricio/Bankei/blob/master/Bankei.png?raw=true)
