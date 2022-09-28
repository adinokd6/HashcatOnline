import { createGlobalStyle, css } from "styled-components"

interface Props {
    theme: string
}

export const GlobalStyles = createGlobalStyle(
    (props: Props) => css`
    :root {
      --color-dark: hsl(0, 0%, 10%);
      --color-light: hsl(0, 0%, 95%);
    }

    body {
      background-color: ${props.theme === "light" ? "var(--color-light)" : "var(--color-dark)"};
      color: ${props.theme === "light" ? "var(--color-dark)" : "var(--color-light)"};
    }
  `,
)