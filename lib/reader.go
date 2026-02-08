package lib

import (
	"bufio"
	"os"
	"strings"
)

func Reader(address string) (string, error) {
	file, err := os.Open(address)

	if err != nil {
		return "", err
	}

	defer file.Close()

	var content strings.Builder
	scanner := bufio.NewScanner(file)

	for scanner.Scan() {
		content.WriteString(scanner.Text() + "\n")
	}

	return content.String(), scanner.Err()
}
