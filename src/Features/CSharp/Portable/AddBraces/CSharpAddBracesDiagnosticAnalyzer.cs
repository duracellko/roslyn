﻿using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.CodeAnalysis.CSharp.Diagnostics.AddBraces
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal sealed class CSharpAddBracesDiagnosticAnalyzer : DiagnosticAnalyzer, IBuiltInAnalyzer
    {
        private static readonly LocalizableString s_localizableTitle =
            new LocalizableResourceString(nameof(FeaturesResources.AddBraces), FeaturesResources.ResourceManager,
                typeof(FeaturesResources));

        private static readonly LocalizableString s_localizableMessage =
            new LocalizableResourceString(nameof(WorkspacesResources.AddBraces), WorkspacesResources.ResourceManager,
                typeof(WorkspacesResources));

        private static readonly DiagnosticDescriptor s_descriptor = new DiagnosticDescriptor(IDEDiagnosticIds.AddBracesDiagnosticId,
                                                                    s_localizableTitle,
                                                                    s_localizableMessage,
                                                                    DiagnosticCategory.Style,
                                                                    DiagnosticSeverity.Info,
                                                                    isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(s_descriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKindsOfInterest);
        }

        public DiagnosticAnalyzerCategory GetAnalyzerCategory() => DiagnosticAnalyzerCategory.SemanticSpanAnalysis;

        private ImmutableArray<SyntaxKind> SyntaxKindsOfInterest { get; } =
            ImmutableArray.Create(SyntaxKind.IfStatement,
                SyntaxKind.ElseClause,
                SyntaxKind.ForStatement,
                SyntaxKind.ForEachStatement,
                SyntaxKind.WhileStatement,
                SyntaxKind.DoStatement,
                SyntaxKind.UsingStatement);

        public void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var node = context.Node;

            switch (node.Kind())
            {
                case SyntaxKind.IfStatement:
                    var ifStatement = (IfStatementSyntax)node;
                    if (AnalyzeIfStatement(ifStatement))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(s_descriptor,
                            ifStatement.IfKeyword.GetLocation(), SyntaxFacts.GetText(SyntaxKind.IfKeyword)));
                    }
                    break;

                case SyntaxKind.ElseClause:
                    var elseClause = (ElseClauseSyntax)node;
                    if (AnalyzeElseClause(elseClause))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(s_descriptor,
                            elseClause.ElseKeyword.GetLocation(), SyntaxFacts.GetText(SyntaxKind.ElseKeyword)));
                    }
                    break;

                case SyntaxKind.ForStatement:
                    var forStatement = (ForStatementSyntax)node;
                    if (AnalyzeForStatement(forStatement))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(s_descriptor,
                            forStatement.ForKeyword.GetLocation(), SyntaxFacts.GetText(SyntaxKind.ForKeyword)));
                    }
                    break;

                case SyntaxKind.ForEachStatement:
                    var forEachStatement = (ForEachStatementSyntax)node;
                    if (AnalyzeForEachStatement(forEachStatement))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(s_descriptor,
                            forEachStatement.ForEachKeyword.GetLocation(), SyntaxFacts.GetText(SyntaxKind.ForEachKeyword)));
                    }
                    break;

                case SyntaxKind.WhileStatement:
                    var whileStatement = (WhileStatementSyntax)node;
                    if (AnalyzeWhileStatement(whileStatement))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(s_descriptor,
                            whileStatement.WhileKeyword.GetLocation(), SyntaxFacts.GetText(SyntaxKind.WhileKeyword)));
                    }
                    break;

                case SyntaxKind.DoStatement:
                    var doStatement = (DoStatementSyntax)node;
                    if (AnalyzeDoStatement(doStatement))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(s_descriptor,
                            doStatement.DoKeyword.GetLocation(), SyntaxFacts.GetText(SyntaxKind.DoKeyword)));
                    }
                    break;

                case SyntaxKind.UsingStatement:
                    var usingStatement = (UsingStatementSyntax)node;
                    if (AnalyzeUsingStatement(usingStatement))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(s_descriptor,
                            usingStatement.UsingKeyword.GetLocation(), SyntaxFacts.GetText(SyntaxKind.UsingKeyword)));
                    }
                    break;
            }
        }

        private bool AnalyzeIfStatement(IfStatementSyntax ifStatement) => 
            !ifStatement.Statement.IsKind(SyntaxKind.Block);

        private bool AnalyzeElseClause(ElseClauseSyntax elseClause) =>
            !elseClause.Statement.IsKind(SyntaxKind.Block) &&
            !elseClause.Statement.IsKind(SyntaxKind.IfStatement);

        private bool AnalyzeForStatement(ForStatementSyntax forStatement) =>
            !forStatement.Statement.IsKind(SyntaxKind.Block);

        private bool AnalyzeForEachStatement(ForEachStatementSyntax forEachStatement) =>
            !forEachStatement.Statement.IsKind(SyntaxKind.Block);
            
        private bool AnalyzeWhileStatement(WhileStatementSyntax whileStatement) =>
            !whileStatement.Statement.IsKind(SyntaxKind.Block);
            
        private bool AnalyzeDoStatement(DoStatementSyntax doStatement) =>
            !doStatement.Statement.IsKind(SyntaxKind.Block);
            
        private bool AnalyzeUsingStatement(UsingStatementSyntax usingStatement) =>
            !usingStatement.Statement.IsKind(SyntaxKind.Block) &&
            !usingStatement.Statement.IsKind(SyntaxKind.UsingStatement);
    }
}