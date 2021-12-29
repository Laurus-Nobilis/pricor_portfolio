<?php
namespace App\Model\Table;

use Cake\ORM\Query;
use Cake\ORM\RulesChecker;
use Cake\ORM\Table;
use Cake\Validation\Validator;

/**
 * Userunit Model
 *
 * @property \App\Model\Table\UsersTable|\Cake\ORM\Association\BelongsTo $Users
 * @property \App\Model\Table\UnitsTable|\Cake\ORM\Association\BelongsTo $Units
 *
 * @method \App\Model\Entity\Userunit get($primaryKey, $options = [])
 * @method \App\Model\Entity\Userunit newEntity($data = null, array $options = [])
 * @method \App\Model\Entity\Userunit[] newEntities(array $data, array $options = [])
 * @method \App\Model\Entity\Userunit|bool save(\Cake\Datasource\EntityInterface $entity, $options = [])
 * @method \App\Model\Entity\Userunit|bool saveOrFail(\Cake\Datasource\EntityInterface $entity, $options = [])
 * @method \App\Model\Entity\Userunit patchEntity(\Cake\Datasource\EntityInterface $entity, array $data, array $options = [])
 * @method \App\Model\Entity\Userunit[] patchEntities($entities, array $data, array $options = [])
 * @method \App\Model\Entity\Userunit findOrCreate($search, callable $callback = null, $options = [])
 */
class UserunitTable extends Table
{

    /**
     * Initialize method
     *
     * @param array $config The configuration for the Table.
     * @return void
     */
    public function initialize(array $config)
    {
        parent::initialize($config);

        $this->setTable('userunit');
        $this->setDisplayField('id');
        $this->setPrimaryKey('id');

        $this->belongsTo('Users', [
            'foreignKey' => 'user_id',
            'joinType' => 'INNER'
        ]);
        $this->belongsTo('Units', [
            'foreignKey' => 'unit_id',
            'joinType' => 'INNER'
        ]);
    }

    /**
     * Default validation rules.
     *
     * @param \Cake\Validation\Validator $validator Validator instance.
     * @return \Cake\Validation\Validator
     */
    public function validationDefault(Validator $validator)
    {
        $validator
            ->integer('id')
            ->allowEmpty('id', 'create');

        $validator
            ->integer('exp')
            ->requirePresence('exp', 'create')
            ->notEmpty('exp');

        $validator
            ->integer('level')
            ->requirePresence('level', 'create')
            ->notEmpty('level');

        $validator
            ->integer('equipment1')
            ->requirePresence('equipment1', 'create')
            ->notEmpty('equipment1');

        $validator
            ->integer('equipment2')
            ->requirePresence('equipment2', 'create')
            ->notEmpty('equipment2');

        $validator
            ->integer('equipment3')
            ->requirePresence('equipment3', 'create')
            ->notEmpty('equipment3');

        $validator
            ->integer('equipment4')
            ->requirePresence('equipment4', 'create')
            ->notEmpty('equipment4');

        return $validator;
    }

    /**
     * Returns a rules checker object that will be used for validating
     * application integrity.
     *
     * @param \Cake\ORM\RulesChecker $rules The rules object to be modified.
     * @return \Cake\ORM\RulesChecker
     */
    public function buildRules(RulesChecker $rules)
    {
        $rules->add($rules->existsIn(['user_id'], 'Users'));
        $rules->add($rules->existsIn(['unit_id'], 'Units'));

        return $rules;
    }

    /**
     * Returns the database connection name to use by default.
     *
     * @return string
     */
    public static function defaultConnectionName()
    {
        return 'user_db';
    }
}
