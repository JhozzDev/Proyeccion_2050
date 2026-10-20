import pandas as pd

csv = pd.read_csv("data/data_set.csv", header=None, sep=';',  encoding="latin-1")
csv = csv.drop(0)
csv = csv.rename(columns={0: "edad", 1: "sexo", 2: "poblacion", 3: "anio"})
csv["poblacion"] = pd.to_numeric(csv["poblacion"], errors='coerce')
csv["anio"] = pd.to_numeric(csv["anio"], errors='coerce')
csv['edad_min'] = pd.to_numeric(csv['edad'].str.extract(r'(\d+)')[0]).astype('Int64')

csv_pivot = csv.pivot_table(index=['anio', 'sexo'], columns='edad_min', values='poblacion', aggfunc='sum').reset_index()

hombres = csv_pivot[csv_pivot["sexo"].str.strip().str.lower() == "hombres"]
mujeres = csv_pivot[csv_pivot["sexo"].str.strip().str.lower() == "mujeres"]
ambos = csv_pivot[csv_pivot["sexo"].str.strip().str.lower() == "ambos sexos"]

hombres = hombres.drop(columns="sexo").set_index("anio")
mujeres = mujeres.drop(columns="sexo").set_index("anio")
ambos = ambos.drop(columns="sexo").set_index("anio")

residuo = mujeres.add(hombres, fill_value=0).subtract(ambos, fill_value=0)

csv.to_csv("data/limpio.csv", index=False)
