import { languages, ExtensionContext, Hover, Position, TextDocument, MarkdownString } from 'vscode';
import hoverExplanations from './hoverExplanations';

export function activate(context: ExtensionContext) {
  // Register the EZCode hover provider
  context.subscriptions.push(
    languages.registerHoverProvider('ezcode', {
      provideHover(document: TextDocument, position: Position): Hover | undefined {
        const wordRange = document.getWordRangeAtPosition(position);
        if (!wordRange) return;

        const word = document.getText(wordRange);
        const explanation = hoverExplanations[word];

        if (explanation) {
          return new Hover(new MarkdownString(explanation));
        }
      },
    })
  );
}
