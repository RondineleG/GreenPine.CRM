# Uinta Pine CRM
senção de responsabilidade: Este é um projeto em andamento. Os aplicativos Blazor do Web Assembly ainda não são totalmente compatíveis no .NET Core 3.0.

Este é um aplicativo CRM simples, criado com a nova estrutura Blazor. Um perfil simples da empresa pode ser criado e os clientes associados a essa empresa. Os clientes podem ter "tags" associadas a eles. Tags representam estado / status / alertas / qualquer coisa. Os nomes de tags são personalizáveis. Uma empresa pode ter outros usuários autorizados, adicionados através do painel de configurações.

Para usar isso, clone o repositório e defina a seqüência de conexão do MongoDB e SigninKey nos arquivos AppSettings.json. Não confirme o MongoDBConnectionString ou SigningKey no controle de origem, em vez disso, coloque-os na configuração do servidor. Ignore as alterações no seu AppSettings.json com estes comandos git:
