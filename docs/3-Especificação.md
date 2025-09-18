
# 3. Especificações do Projeto

<span style="color:red">Pré-requisitos: <a href="2-Planejamento-Projeto.md"> Planejamento do Projeto do Software (Cronograma) </a></span>

> Nesta seção, você vai detalhar os requisitos do seu sistema e as restrições do projeto, organizando as funcionalidades e características que a solução deve ter.

---

## 3.1 Requisitos Funcionais

Preencha o Quadro abaixo com os requisitos funcionais que **detalham as funcionalidades que seu sistema deverá oferecer**.  
Cada requisito deve representar uma característica única da solução e ser claro para orientar o desenvolvimento.


|ID     | Descrição do Requisito                                                                                            | Prioridade |
|-------|-----------------------------------------------------------------------------------------------------------------|------------|
|RF-01  | O sistema deve permitir que os usuários criem uma conta informando nome, e-mail e senha.                        | ALTA       | 
|RF-02  | O sistema deve permitir que os usuários recuperem sua senha caso a esqueçam.                                   | ALTA       |
|RF-03  | Apenas usuários cadastrados podem acessar os conteúdos.                                                          | ALTA       |
|RF-04  | O sistema deve permitir que os usuários gerenciem seu perfil, podendo editar foto, nome, áreas de interesse e informações de login. | MÉDIA      |
|RF-05  | O sistema deve permitir que os usuários selecionem preferências de tipos de conteúdo.                             | MÉDIA      |
|RF-06  | O sistema deve oferecer uma classificação de credibilidade de cada perfil.                                        | MÉDIA      |
|RF-07  | O sistema deve permitir a publicação de conteúdos de dois tipos: vídeos e perguntas.                             | ALTA       |
|RF-08  | O sistema deve permitir que os usuários busquem por conteúdos com base em filtros predefinidos.                    | ALTA       |
|RF-09  | O sistema deve exibir conteúdos recomendados priorizando maior classificação de credibilidade e compatibilidade com os interesses do usuário. | MÉDIA |

---

## 3.2 Histórias de Usuário

> Cada história de usuário deve ser escrita no formato:  
>  
> **Como [persona], eu quero [funcionalidade], para que [benefício/motivo].**  seguindo o modelo e conceitos ensinados na disciplina de       
> Engenharia de Requisitos.   
---
⚠️ **ATENÇÃO:** Escreva de forma que cada história de usuário esteja associada a um requisito funcional específico para facilitar o acompanhamento e validação. Por exemplo:

> **História 1 (relacionada ao Requisito RF-001):**  
> Como usuário, quero registrar minhas tarefas para não esquecer de fazê-las.  
>  
> **História 2 (relacionada ao Requisito RF-002):**  
> Como administrador, quero alterar permissões para controlar o acesso ao sistema.  
>  
> Para melhor organização, as histórias podem ser agrupadas por contexto ou módulo funcional.

---

### ✏️ Escreva aqui as histórias de usuário do seu projeto:

<div style="border: 2px dashed #999999; padding: 15px; margin: 10px 0;">
  
<!-- Espaço para escrever o texto -->  
**[Escreva aqui as histórias do seu projeto]**

- **História 1 (relacionada ao Requisito RF-01):** 

- **História 2 (relacionada ao Requisito RF-02):** 




</div>

---

## 3.3 Requisitos Não Funcionais

Preencha o Quadro abaixo com os requisitos não funcionais que definem **características desejadas para o sistema que irão desenvolver**, como desempenho, segurança, usabilidade, etc.  
> Lembre-se que esses requisitos são importantes para garantir a qualidade da solução.

|ID     | Descrição do Requisito                                                                              |Prioridade |
|-------|-----------------------------------------------------------------------------------------------------|-----------|
|RNF-01 | O sistema deve carregar as páginas em até 3 segundos para garantir uma boa experiência ao usuário.  | MÉDIA     | 
|RNF-02 | O sistema deve proteger as informações dos clientes por meio de criptografia e medidas de segurança.| ALTA      | 
|RNF-03 | O sistema deve ser responsivo, adaptando-se a diferentes dispositivos (computadores, tablets e smartphones).                                     | ALTA  |
|RNF-04 | O sistema deve estar disponível em pelo menos 95% do tempo, garantindo confiabilidade no acesso.                                       | ALTA  |
|RNF-05 | O sistema deve ser compatível com os navegadores mais utilizados (Chrome, Firefox, Edge e Safari).                                      | MÉDIA |
|RNF-06 | O sistema deve realizar backup automático do banco de dados diariamente para evitar perda de informações.                                       | ALTA  |
|RNF-07 | O sistema deve apresentar interface intuitiva e acessível, seguindo recomendações de acessibilidade digital (WCAG).                                       | MÉDIA  |
|RNF-08 | O tempo de busca por aulas ou usuários deve ser de no máximo 2 segundos.                                      | MÉDIA  |
|RNF-09 | O sistema deve permitir fácil manutenção e evolução, adotando boas práticas de programação e versionamento (Git).                                       | MÉDIA  |
|RNF-10 | O sistema deve suportar até 200 usuários simultâneos sem degradação perceptível de desempenho.                                       | ALTA  |
|RNF-11 | O sistema deve exibir mensagens de erro claras e não técnicas em caso de falha.                                       | BAIXA  |
|RNF-12 | O sistema deve ser desenvolvido em arquitetura modular, facilitando a adição de novas funcionalidades.                                       | MÉDIA  |

---

## 3.4 Restrições do Projeto – EduTroca


| ID   | Restrição                                                                 |
|------|----------------------------------------------------------------------------|
| R-01 | O projeto deverá ser entregue até o final do semestre.                     |
| R-02 | O sistema deve funcionar apenas dentro da rede interna da empresa.         |
| R-03 | O software deve ser compatível com Windows e Linux.                        |
| R-04 | O sistema deve ser desenvolvido utilizando tecnologias web responsivas.    |
| R-05 | A plataforma deve estar disponível 24/7, salvo períodos de manutenção.     |
| R-06 | O acesso ao sistema só poderá ser feito mediante autenticação segura.      |
| R-07 | Não é permitido integrar métodos de pagamento, já que não envolve dinheiro.|
| R-08 | Alterações no escopo após o início do semestre só poderão ser aprovadas pelo professor/orientador. |

---
## 3.5 Regras de Negócio

> Regras de Negócio definem as condições e políticas que o sistema deve seguir para garantir o correto funcionamento alinhado ao negócio.  
>  
> Elas indicam **quando** e **como** certas ações devem ocorrer, usando o padrão:  
>  
> **Se (condição) for verdadeira, então (ação) deve ser tomada.**  
>  
> Exemplo:  
> - "Um usuário só poderá finalizar um cadastro se todos os dados forem inseridos e validados com sucesso."  
>  
> Também pode ser escrito assim (if/then):  
> - "Se o usuário tem saldo acima de X, então a opção de empréstimo estará liberada."

---

 A tabela abaixo deve ser preenchida com as regras de negócio que **impactam seu projeto**. Os textos no quadro são apenas ilustrativos.

| ID    | Regra de Negócio                                                                 | Justificativa/Objetivo                                                                 |
|-------|----------------------------------------------------------------------------------|----------------------------------------------------------------------------------------|
| RN-01 | Somente usuários cadastrados podem acessar e publicar conteúdos.                  | Garantir segurança e controle de acesso à comunidade.                                  |
| RN-02 | Cada conteúdo publicado gera créditos que podem ser usados para consumir outros. | Incentivar a colaboração e troca de conhecimento.                                     |
| RN-03 | O sistema deve atribuir pontos e níveis de reconhecimento por engajamento.       | Estimular gamificação e participação ativa dos usuários.                              |
| RN-04 | O usuário só poderá acessar conteúdos se possuir créditos suficientes.           | Manter equilíbrio no uso da plataforma e incentivar contribuição.                     |
| RN-05 | Os vídeos publicados ficam disponíveis sem necessidade de agendamento.           | Facilitar acesso contínuo ao conteúdo e promover flexibilidade de aprendizado.         |
| RN-06 | A plataforma deve ser simples e acessível a todos.                               | Promover inclusão digital e garantir usabilidade.                                      |
| RN-07 | Usuários devem interagir de forma respeitosa; conteúdos ofensivos são proibidos. | Manter ambiente saudável, ético e seguro na comunidade.                               |
| RN-08 | A plataforma não prioriza usuários específicos; relevância define a visibilidade.| Garantir imparcialidade e justiça na recomendação de conteúdos.                       |
| RN-09 | Créditos não podem ser convertidos em dinheiro real.                             | Evitar uso comercial indevido e manter o foco educacional da plataforma.              |
| RN-10 | Dados dos usuários devem ser tratados de forma segura.                           | Atender a requisitos legais de privacidade e garantir confiança dos participantes.     |

---
> **Links Úteis**:
> - [O que são Requisitos Funcionais e Requisitos Não Funcionais?](https://codificar.com.br/requisitos-funcionais-nao-funcionais/)
> - [O que são requisitos funcionais e requisitos não funcionais?](https://analisederequisitos.com.br/requisitos-funcionais-e-requisitos-nao-funcionais-o-que-sao/)
